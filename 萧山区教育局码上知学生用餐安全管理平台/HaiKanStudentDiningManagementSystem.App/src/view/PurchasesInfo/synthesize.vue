<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.demo.query.totalCount"
        :pageSize="stores.demo.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="23">
              <Col span="23" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
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
          :data="stores.demo.data"
          :columns="stores.demo.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="error"
                  v-can="'delete'"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
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
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
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
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="时间" prop="createdAt">
            <Date-picker
              v-model="formModel.fields.createdAt"
              @on-change="formModel.fields.createdAt = $event"
              type="datetime"
              placeholder="请选择时间"
              style="width: 100%"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="主题" prop="subjectId">
            <Select
              filterable
              v-model="formModel.fields.subjectId"
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="GetTypes"
            >
              <Option
                v-for="item in typesList"
                :key="item.typeId"
                :value="item.typeId"
              >{{ item.description }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="活动负责人" prop="repersonId">
            <Select
              filterable
              v-model="formModel.fields.repersonId"
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="Getreperson"
            >
              <Option
                v-for="item in usersList"
                :key="item.userId"
                :value="item.userId"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="持续时间/分钟" prop="continueTime">
            <Input-number
              v-model="formModel.fields.continueTime"
              :min="0"
              :readonly="checkShow()"
              placeholder="请输入持续时间"
              style="width:100%;"
            ></Input-number>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="参与部门" prop="department">
            <Select
              filterable
              v-model="model1"
              multiple
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="Getdepart"
            >
              <Option
                v-for="item in departList"
                :key="item.id"
                :value="item.id"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="创建人" prop="createUserId">
            <Select
              filterable
              v-model="formModel.fields.createUserId"
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
            >
              <Option
                v-for="item in usersList"
                :key="item.sysUserId"
                :value="item.sysUserId"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="参与人数" prop="number">
            <Input-number
              v-model="formModel.fields.number"
              :min="0"
              :readonly="checkShow()"
              placeholder="请输入参与人数"
              style="width:100%;"
            ></Input-number>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="活动介绍" prop="introduce">
            <Input
              type="textarea"
              v-model="formModel.fields.introduce"
              :readonly="checkShow()"
              placeholder="请输入活动介绍"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="result">
            <Input
              type="textarea"
              v-model="formModel.fields.result"
              :readonly="checkShow()"
              placeholder="请输入备注"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left;"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  type="ios-trash-outline"
                  style="float: right;"
                  @click.native="handleRemove(item.name)"
                   v-show="!checkShow()"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{scene:'采购图片',groupType:'purchase'}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          v-if="!checkShow()"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  GetList, //显示列表
  GetCreate, //新增
  GetShow, //获取选定信息
  batchCommand, //批量删除
  deleteDepartment, //单个删除
  GetEdit, //编辑
  GetTypesList,
  GetDepartList,
  GetUsersList,
} from "@/api/PurchasesInfo/synthesizes";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "synthesizes",
  components: {
    DzTable,
  },
  data() {
    return {
      options3: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
      },

      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,

      typesList: [],
      departList:[],
      usersList:[],
      model1: [],
      stores: {
        demo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
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
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "id" },
            { title: "主题", key: "subjectName", sortable: true, ellipsis: true },
            { title: "时间", key: "createdAt", sortable: true, ellipsis: true },
            { title: "部门", key: "departmentName", sortable: true, ellipsis: true },
            { title: "活动介绍", key: "introduce", sortable: true, ellipsis: true },
            { title: "持续时间", key: "continueTime", sortable: true, ellipsis: true },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          imgs: "",
          subjectId: "",
          subjectName: "",
          continueTime: 0,
          department: "",
          departmentName: "",
          reperson:"",
          repersonId:"",
          introduce: "",
          result: "",
          number: 0,
          createUserId:0,
          createdAt:""
        },
        rules: {
          subjectId: [
            { type: "string", required: true, message: "主题不能为空" },
          ],
          createdAt: [
            { required: true, message: "时间不能为空" },
          ],
          repersonId: [
            { type: "string", required: true, message: "活动负责人不能为空" },
          ],
          department: [
            { type: "string", required: true, message: "参与部门不能为空" },
          ],
          createUserId: [
            { required: true, message: "创建人不能为空" },
          ],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel.mode === "show") {
        return "信息详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.id);
    }, //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      GetList(this.stores.demo.query).then((res) => {
        console.log(res);
        this.stores.demo.data = res.data.data;
        this.stores.demo.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.demo.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.demo.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    //主题
    doGetTypesList() {
      GetTypesList().then((res) => {
        this.typesList = res.data.data;
      });
    },
    GetTypes(e){
      if (e != undefined) {
        this.formModel.fields.subjectName = e.label;
      }
    },
    //部门
    doGetDepartList(){
        GetDepartList().then(res=>{
      console.log(res);
            this.departList = res.data.data;
        })
    },
    Getdepart(e){
      let data = [];
      console.log(e);
      for (let k = 0; k < e.length; k++) {
        data.push(e[k].label);
      }
      this.formModel.fields.departmentName = data.join(",");
      this.formModel.fields.department = this.model1.join(",") + ",";
    },
    //人员
    doGetUsersList(){
        GetUsersList().then(res=>{
            console.log(res)
            this.usersList = res.data.data;
        })
    },
    Getreperson(e){
        if (e != undefined) {
        this.formModel.fields.reperson = e.label;
      }
    },
    //详情显示
    handleDetail(e) {
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(e.id);
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
      this.doGetTypesList();
      this.doGetDepartList();
      this.doGetUsersList();
    },
    //右边编辑
    handleEdit(row) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(row.id);
    },
    //查询当前行信息
    doLoadData(id) {
      GetShow(id).then((res) => {
        if (res.data.code === 200) {
          this.model1 = [];
          this.formModel.fields = res.data.data;
          if (res.data.data.department != null) {
            if (res.data.data.department.split(",").length > 0) {
              for (let k = 0;k < res.data.data.department.split(",").length;k++) {
                this.model1.push(parseInt(res.data.data.department.split(",")[k]));
              }
            }
          }
          if (res.data.data.imgs != null) {
            let list = res.data.data.imgs.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  list[i],
                status: "finished",
                name: list[i],
                fileName: list[i]
              });
            }
          }
          console.log(res.data.data);
          this.doGetTypesList();
          this.doGetDepartList();
          this.doGetUsersList();
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateUserForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreateDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditDispatch();
        }
      }
    },
    //添加（保存）
    docreateDispatch() {
      GetCreate(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      if (
        this.formModel.fields.createdAt != "" &&
        this.formModel.fields.createdAt != null
      ) {
        this.formModel.fields.createdAt = new Date(
          Date.parse(new Date(this.formModel.fields.createdAt)) +
            8 * 3600 * 1000
        );
      }
      GetEdit(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.model1=[];
      this.formModel.fields.imgs="";
      this.defaultList=[];
      this.uploadList=[];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.id);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
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
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.imgs = this.uploadList
            .map(x => x.fileName)
            .join(",");
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    toUpResult() {
      console.log(1111);
      console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields.imgs);
        if (this.formModel.fields.imgs != null) {
          if (this.formModel.fields.imgs.length > 0) {
            this.formModel.fields.imgs += ",";
          }
          this.formModel.fields.imgs += response.data;
        } else {
          this.formModel.fields.imgs = response.data;
        }
        await this.uploadList.push({
          url: response.data.replace("\\", "/"),
          status: "finished",
          name: response.data,
          fileName: response.data
        });
      } else {
        this.$Message.warning(response.message);
      }
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/UpImage/ToUPPhoto/UpPhoto";
  },
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>