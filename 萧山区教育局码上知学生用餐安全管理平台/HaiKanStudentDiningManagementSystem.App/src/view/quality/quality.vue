<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.user.query.totalCount"
        :pageSize="stores.user.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw"
                      placeholder="请输入文件名称"
                      @on-search="handleSearchUser()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup>
                  <Button
                    v-can="'batch'"
                    v-show="schoolshow1==1"
                    class="txt-danger"
                    icon="md-trash"
                    title="批量删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button  icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  v-show="schoolshow1==1"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.user.data"
          :columns="stores.user.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
                v-show="schoolshow1==1"
              >
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
              v-show="schoolshow1==1"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"

      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formUser" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <FormItem label="名称" prop="flieName">
            <Input
              v-model="formModel.fields.flieName"
              :readonly="this.formModel.mode=='show'"
              placeholder="请输入文件名称"
            />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="简介" prop="jianJie">
            <Input
              :autosize="{minRows: 2,maxRows: 10}"
              v-model="formModel.fields.jianJie"
              placeholder="请输入文件简介"
              :readonly="this.formModel.mode=='show'"
              type="textarea"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="生效时间" prop="effectiveTime">
            <DatePicker
              type="date"
              placeholder="请选择生效时间"
              v-model="formModel.fields.effectiveTime"
              :editable="false"
              :disabled="this.formModel.mode=='show'"
              style="width: 300px"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <!-- <FormItem label="内容" prop="jianJie"> -->
          <!-- <Input :autosize="{minRows: 2,maxRows: 10}" v-model="formModel.fields.jianJie" placeholder="请输入文件简介" type="textarea"/> -->
          <!-- </FormItem> -->
          <quill-editor
            @focus="onEditorFocus($event)"
            class="mec"
            v-model="formModel.fields.accessory"
          ></quill-editor>
        </Row>
        <!-- <Row v-show="!checkShow()">
          <Upload
            ref="upload"
            :action="actionurl"
            :headers="postheaders"
            :show-upload-list="true"
            :data="{filename:this.formModel.dFileName}"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            style="float:left"
            :disabled="updisabled"
            :on-remove="deleteFile"
          >
            <Button type="primary" icon="ios-cloud-upload-outline" :loading="loadingStatus">上传质量文件：</Button>
          </Upload>
        </Row>-->
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 150px;">
        <Button
          style="margin-top: 50px"
          icon="md-checkmark-circle"
          type="primary"
          v-if="!isreadonly()"
          @click="handleSubmitUser"
        >保 存</Button>
        <Button
          style="margin-left: 8px;margin-top: 50px"
          icon="md-close"
          @click="formModel.opened = false"
        >取 消</Button>
      </div>
    </Drawer>
    <Drawer title="详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formdispatch" label-position="left">
        <Row :gutter="16">
          <FormItem label="文件名称" prop="flieName">
            <Input readonly v-model="formModel1.fields.flieName" placeholder="请输入文件名称" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="培训时间">
            <Input v-model="formModel1.fields.effectiveTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="文件简介" prop="jianJie">
            <Input
              readonly
              :autosize="true"
              v-model="formModel1.fields.jianJie"
              placeholder="请输入文件简介"
              type="textarea"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="附件：" prop="accessory">
            <Card>
              <p slot="title">文件</p>
              <p>{{this.formModel.fields.accessory.split('\\')[1]}}</p>
              <Button @click="download">下载</Button>
            </Card>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  qualityList,
  Create,
  Batch,
  Delete,
  QualityGet,
  QualityEdit,
} from "@/api/standard/standard";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "rbac_user_page",
  components: {
    DzTable,
  },
  data() {
    return {
      url: "",
      updisabled: false,
      loadingStatus: false,
      actionurl: "",
      postheaders: "",
      schoolshow1: 0,
      commands: {
        delete: { name: "delete", title: "删除" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          qualityUuid: "",
          flieName: "",
          jianJie: "",
          addTime: "",
          addPeople: "",
          accessory: "",
          schoolUuid: "",
          effectiveTime: "",
          //avatar: "",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDelete: 0,
        },
        rules: {
          flieName:[{type: "string",required: true,message: "请输入名称",trigger:'blur'}],
           jianJie:[{type: "string",required: true,message: "请输入简介",trigger:'blur'}],
           effectiveTime:[{type: "date",required: true,message: "请输入生效时间",trigger:'blur'}],
          // flieName: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入标题"}],
          // jianJie: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入内容" }],
        },
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
          qualityUuid: "",
          flieName: "",

          jianJie: "",
          addTime: "",
          addPeople: "",
          accessory: "",
          effectiveTime: "",
          //avatar: "",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDelete: 0,
        },
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: [],
      },
      stores: {
        user: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            schoolguid: "",
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
          },
          columns: [
            { type: "selection", width: 50, key: "qualityUuid" },
            { title: "文件名称", key: "flieName", sortable: true,required:"请填写文件名称" },
            { title: "简介", key: "jianJie", ellipsis: true,required:"请填写简介" },
            { title: "生效时间", key: "effectiveTime",required:"请填写日期" },
            // { title: "状态", key: "state" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
      rolelist: [],
      departmentlist: [],
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建";
      }
      if (this.formModel.mode === "show") {
        return "详情";
      }
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.qualityUuid);
    },
  },
  methods: {
    //展示校园新闻列表
    loadUserList() {
      if (this.$store.state.user.schoolguid == null) {
        this.schoolshow1 = 0;
      } else {
        this.schoolshow1 = 1;
      }
      
      console.log(this.schoolshow1);
      //赋值222
      this.stores.user.query.schoolguid = this.$store.state.user.schoolguid;
      qualityList(this.stores.user.query).then((res) => {
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
    },
    toUpResult() {
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    deleteFile(e) {
      console.log(e);
      console.log(this.formModel.dFileName);
      // if (this.formModel.dFileName != null && this.formModel.dFileName != "") {
      //   deletetoFile({ filename: this.formModel.dFileName }).then(res => {
      //     console.log(res);
      //   });
      // }
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    checkShow() {
      return this.formModel.mode === "show"||this.formModel.mode === "edit";
    },
    isreadonly(){
      return this.formModel.mode === "show";
    },
    showUpResult(e) {
      this.loadingStatus = false;
      this.updisabled = false;
      console.log(e);
      if (e.code == "200") {
        this.$Message.success(e.message);
        this.formModel.fields.accessory = e.data.dataPath;
        this.formModel.dFileName = e.data.path;
        console.log(
          (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(e.message);
      }
    },
    //编辑按钮
    handleEdit(row) {
      console.log("点击了编辑")
      this.formModel.fields.accessory = "";
      this.formModel.dFileName = "";
      // this.$refs.upload.fileList = [
      //   { name: "", status: "finished", showProgress: false }
      // ];
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormUser();
      this.doLoadData(row.qualityUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },

    handleShowCreateWindow() {
      this.formModel.fields.accessory = "";
      this.formModel.dFileName = "";
      // this.$refs.upload.clearFiles();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
    },
    handleSearch() {
      this.loadUserList();
    },
    //编辑/保存
    handleSubmitUser() {
      if (this.formModel.fields.flieName == "") {
        this.$Message.warning("文件名称不能为空!");
        return;
      }
      if (this.formModel.fields.effectiveTime == "") {
        this.$Message.warning("生效时间不能为空!");
        return;
      }
      //console.log(this.formModel.fields.effectiveTime);
      //return;
      // if (this.formModel.fields.jianJie=="") {
      //   this.$Message.warning("文件名称不能为空!");
      //   return;
      // }
      // if (this.formModel.dFileName=="") {
      //   this.$Message.warning("文件不能为空!");
      //   return;
      // }
      if (this.formModel.mode === "create") {
        this.docreateDispatch();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
    docreateDispatch() {
      this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
      Create(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
      //
    },
    //编辑信息（保存）
    doEditDispatch() {
      QualityEdit(this.formModel.fields).then((res) => {
        console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleResetFormUser() {
      this.$refs["formUser"].resetFields();
      this.formModel.fields.effectiveTime = "";
      this.formModel.fields.flieName = "";
      this.formModel.dFileName = "";
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formUser"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadUser(systemUserUuid) {
      loadUser({ guid: systemUserUuid }).then((res) => {
        this.formModel.fields = res.data.data.query;
      });
    },
    //删除单个用户
    handleDelete(row) {
      this.doDelete(row.qualityUuid);
    },
    doDelete(ids) {
      Delete(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //批量删除
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    doBatchCommand(command) {
      Batch({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchUser() {
      this.loadUserList();
    },
    //右边详情
    handleDetail(row) {
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.doLoadData(row.qualityUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      QualityGet(id).then((res) => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.formModel1.fields = res.data.data[0];
        }
        //  this.$refs.upload.fileList[0].name = this.formModel.fields.accessory.split(
        //   "\\"
        // )[1];
        this.formModel.flieName = res.data.data.path;
        this.url =
          config.baseUrl.dev +
          this.formModel.fields.accessory.replace("\\", "/");
      });
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadUserList();
    },
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadUserList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadUserList();
    },
    download() {
      // console.log(this.url);
      window.location.href =
        config.baseUrl.dev + this.formModel.fields.accessory.replace("\\", "/");
    },
    onEditorFocus(event) {
      if (this.formModel.mode === "show") {
        event.enable(false);
      } else {
        event.enable(true);
      }
    }, // 获得焦点事件
  },
  mounted() {
    
    this.loadUserList();
    //this.doloadDepartmentListdetail();
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/Quality/Quality/UpLoad";
  },
};
</script>
<style>
.mec {
  height: 300px;
}
</style>
