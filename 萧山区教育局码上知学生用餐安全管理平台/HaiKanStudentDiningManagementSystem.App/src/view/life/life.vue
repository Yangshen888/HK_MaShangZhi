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
                      placeholder="请输入标题"
                      @on-search="handleSearchUser()"
                    ></Input>                   
                  </FormItem>
                  <FormItem>
                    <Input 
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw1"
                      placeholder="请输入标签"
                      @on-search="handleSearchUser()"
                    ></Input>                   
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <Button
                    v-can="'batch'"
                    v-show="schoolshow1==1"
                    class="txt-danger"
                    icon="md-trash"
                    title="批量删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button  icon="md-refresh" title="刷新" @click="handleRefresh" ></Button>
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
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
              >
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true" v-show="schoolshow1==1">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true" v-show="schoolshow1==1">
              <Button v-can="'edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
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
      width="400"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formUser" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
            <FormItem label="标题" prop="headline">
              <Input v-model="formModel.fields.headline" placeholder="请输入标题"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="摘要" prop="digest">
            <Input
              v-model="formModel.fields.digest"
              placeholder="请输入摘要"
            />
          </FormItem>
        </Row>       
         <Row :gutter="16">
            <FormItem label="标签" prop="tag">
              <Input v-model="formModel.fields.tag"  placeholder="请输入标签" maxlength="2"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="图片上传:" >
              <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img style="width:118px;height:118px;" :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
                <Icon type="ios-trash-outline" @click.native="handleRemove(item.name)"></Icon>
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
            :data="{uuid:this.$store.state.user.userGuid}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            :headers="postheaders"
            type="drag"
            :action="actionurl"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
            </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formUser" label-position="left">
        
        <Row :gutter="16">
          <FormItem label="标题">
            <Input v-model="formModel1.fields.headline" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="摘要" prop="digest">
            <Input
              :readonly="true"
              v-model="formModel.fields.digest"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="标签">
            <Input v-model="formModel1.fields.tag" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="日期">
            <Input v-model="formModel1.fields.addTime" :readonly="true" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="内容">
            <Input type="textarea" v-model="formModel1.fields.content" :readonly="true" />
          </FormItem>
        </Row> -->
        <Row>
            <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" style="width:118px;height:118px;"/>
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
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
  SchoolLifeList,
  Create,
  Batch,
  Delete,
 SchoolLifeGet,
 SchoolLifeEdit,
  DeletetoFile
} from "@/api/schoollife/schoollife";

import {
    globalvalidateIDCardNoMust,
    globalvalidatepwd,
    globalvalidateIsNotEmpty
    } from "@/global/validate";
        import { getToken } from "@/libs/util";
    import config from "@/config";
export default {
  name: "rbac_user_page",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
        //附件上传
      imgshow1: false,
      img1: "",
      img: [],
     
      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,
      schoolshow1:0,
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          schoolUuid:"",
          schoolLifeUuid:"",
          headline:"",
          // content:"",
          addTime:"",
          addPeople:"",
          accessory: "",
          tag: "",
          digest:"",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDeleted: 0,
            systemRoleUuid:'',
            userIdCard:'',
            departmentUuid:''
        },
        rules: {
          // headline: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入标题"}],
          // content: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入内容" }],
          headline: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入标题"}],
          digest: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入摘要"}],
          tag: [{ validator:globalvalidateIsNotEmpty,type: "string", required: true, message: "请输入标签"}],
        }
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
          headline:"",
          // content:"",
          addTime:"",
          loginName: "",
          accessory:"",
          realName: "",
          passWord: "",
          addPeople:"",
          tag: "",
          digest:"",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDeleted: 0
        }
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      stores: {
        user: {
          query: {
            totalCount: 0,
            schoolguid:"",
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" }
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "schoolLifeUuid" },
            { title: "标题", key: "headline",  sortable: true },
            { title: "摘要", key: "digest",  sortable: true },
            { title: "标签", key: "tag",  },
              { title: "日期", key: "addTime",  },
            // { title: "状态", key: "state" },
            { title: "操作", align: "center", width: 150, className: "table-command-column",slot:"action" }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      },
        rolelist:[],
        departmentlist:[]
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建";
      }
       if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.schoollifeUuid);
    }
  },
  methods: {
      //展示校园新闻列表
    loadUserList() {
       if (this.$store.state.user.schoolguid == null) {
                this.schoolshow1=0;
                
      }else{
         this.schoolshow1=1;
      }
       this.stores.user.query.schoolguid = this.$store.state.user.schoolguid;
      SchoolLifeList(this.stores.user.query).then(res => {
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
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
    
    //编辑按钮
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormUser();
      this.doLoadData(row.schoollifeUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },

    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();

    },
        handleSearch(){
        this.loadUserList();
    },
    //编辑/保存
    handleSubmitUser() {
      if (this.formModel.fields.headline=="") {
        this.$Message.warning("标题不能为空!");
        return;
      }
      if (this.formModel.fields.digest == "") {
        this.$Message.warning("摘要不能为空!");
        return;
      }
      if (this.formModel.fields.tag == "") {
        this.$Message.warning("标签不能为空!");
        return;
      }
       if (this.formModel.fields.accessory=="") {
        this.$Message.warning("图片不能为空!");
        return;
      }
      if (this.formModel.mode === "create") {
        //alert(this.formModel.fields.schoolUuid);
        this.docreateDispatch();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
     docreateDispatch() {
       this.formModel.fields.schoolUuid=this.$store.state.user.schoolguid;
      Create(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑信息（保存）
    doEditDispatch() {
      SchoolLifeEdit(this.formModel.fields).then(res => {
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
      this.formModel.fields.headline = "";
      this.formModel.fields.addTime = "";
      this.formModel.fields.tag = "";
       this.uploadList=[];
      this.formModel.fields.accessory = "";
       this.img1 = "";
      this.img = [];
      this.imgshow1 = false; 
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formUser"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //删除单个用户
    handleDelete(row) {
      this.doDelete(row.schoollifeUuid);
    },
    doDelete(ids) {
      Delete(ids).then(res => {
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
        }
      });
    },
    doBatchCommand(command) {
      Batch({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
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
        handleSearchUser(){
     this.loadUserList();
    },
    //右边详情
    handleDetail(row) {
      this.formModel1.opened = true;
      this.doLoadData(row.schoollifeUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      SchoolLifeGet(id).then(res => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.formModel1.fields = res.data.data[0];         
             let list = this.formModel.fields.accessory.split(",");
        this.uploadList = [];
        if(list[0]!=""){
for (let i = 0; i < list.length; i++) {
          this.uploadList.push({
            url: config.baseUrl.dev + "UploadFiles/Picture/" + list[i],
            status: "finished",
            name: "UploadFiles/Picture/" + list[i],
            fileName: list[i]
          });
        }
        }
        }
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
checkShow() {
      return this.formModel.mode === "show";
    },
    
    async showUpResult(response, file, fileList) {
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        if (this.formModel.fields.accessory.length > 0) {
          this.formModel.fields.accessory += ",";
          
        }
        this.formModel.fields.accessory += response.data.fname;
        // this.formModel.dFileName = response.data.paths;

        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname
        });
 
      } else {
        this.$Message.warning(e.message);
      }
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
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName)
    },
    handleRemove(file) {
      DeletetoFile({ path: file }).then(res => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        } else {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        }
      });
    },
  },
  mounted() {
    this.loadUserList();
    //this.doloadDepartmentListdetail();
  },
    created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/NewsReport/SchoolLife/UpLoad";
  }
};
</script>

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
