<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.liveShot.query.totalCount"
        :pageSize="stores.liveShot.query.pageSize"
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
                      v-model="stores.liveShot.query.kw"
                      placeholder="输入菜品名称搜索..."
                      @on-search="handleSearchLiveShot()"
                    />
                  </FormItem>
                  <!-- <FormItem>
                    <DatePicker
                      v-model="stores.liveShot.query.kw2"
                      type="daterange"
                      placeholder="视频时间"
                      format="yyyy/MM/dd"
                      style="width: 200px"
                      @on-change="dateChage"
                    ></DatePicker>
                  </FormItem>-->
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-hand"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('forbidden')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-checkmark"-->
                  <!--                    title="启用"-->
                  <!--                    @click="handleBatchCommand('normal')"-->
                  <!--                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增实拍"
                >新增实拍</Button>
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
          :data="stores.liveShot.data"
          :columns="stores.liveShot.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>-->
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.state).color">{{renderStatus(row.state).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
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
                @click="handleShow(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
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
      <Form
        :model="formModel.fields"
        ref="formLiveShot"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="对应日期" prop="datetime">
              <DatePicker
                :disabled="checkShow()"
                v-model="formModel.fields.datetime"
                type="date"
                placeholder="选择日期"
                style="width: 250px"
                @on-change="toSearchCuisineList"
              ></DatePicker>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="用餐类型" prop="datetype">
              <Select
                :disabled="checkShow()"
                v-model="formModel.fields.datetype"
                @on-change="toSearchCuisineList"
              >
                <Option
                  v-for="item in stores.liveShot.sources.dateType"
                  :value="item.value"
                  :key="item.value"
                >{{item.text}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="菜品名称" prop="cuisineUuid">
            <Select
              :disabled="checkShow()"
              v-model="formModel.fields.cuisineUuid"
              @on-change="changeCuisineName"
            >
              <Option
                v-for="item in formModel.cuisines"
                :value="item.cuisineUuid"
                :key="item.cuisineUuid"
              >{{item.cuisineName}}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="主料" prop="ingredient">
            <Input readonly v-model="formModel.fields.ingredient" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="配料" prop="burdening">
            <Input readonly v-model="formModel.fields.burdening" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="abstract">
            <Input readonly :autosize="true" v-model="formModel.fields.abstract" type="textarea" />
          </FormItem>
        </Row>

        <!-- <Row :gutter="16">
          <FormItem label="附件" prop="accessory">
            <img
              v-if="imgshow1==false ? true : false"
              src="../../assets/images/bookone.jpg"
              class="fileimg1"
            />
            <div v-if="imgshow1">
              <img :src="img1" class="fileimg1" />
            </div>
            <input
              type="file"
              @change="addImg1"
              ref="inputer1"
              multiple
              accept="image/png, image/jpeg, image/gif, image/jpg"
            />
          </FormItem>
        </Row>-->
        <Row v-show="!checkShow()">
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
            :data="{uuid:this.$store.state.user.userGuid}"
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
        <Row v-if="checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
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
      <div v-if="!checkShow()" class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitLiveShot(0)">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getLiveShotList,
  createLiveShot,
  loadLiveShot,
  editLiveShot,
  deleteLiveShot,
  batchCommand,
  deletetoFile,
  getRegistPicture,
  cuisineSelectList
} from "@/api/diningRoom/liveShot";
// import { getStaffList } from "@/api/rbac/user";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty
} from "@/global/validate";
import { DateToString } from "@/global/utils";
// import { cuisineSelectList } from "@/api/cuisine/cuisine";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "liveShot",
  components: {
    DzTable
  },
  data() {
    return {
      imgshow1: false,
      img1: "",
      img: [],
      url: config.baseUrl.dev,
      imgName: "",
      visible: false,
      updisabled: false,
      uploadList: [],
      defaultList: [],
      loadingStatus: false,
      actionurl: "",
      postheaders: "",
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        selectPeople: [],
        cuisines: [],
        fields: {
          liveShotUuid: "",
          cuisineUuid: "",
          ingredient: "",
          burdening: "",
          abstract: "",
          accessory: "",
          addTime: "",
          addPeople: "",
          isDelete: "",
          schoolUuid: "",
          datetime: "",
          datetype: "",
          schoolName:"",
        },
        rules: {
          name: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入视频名称"
            }
          ],
          datetime: [
            {
              type: "date",
              required: true,
              message: "选择时间",
              trigger: "change"
            }
          ],
          datetype: [{ required: true, message: "请选择类型" }],
          cuisineUuid: [{ required: true, message: "请选择菜品" }],
          selectPeople: [{ required: true, message: "请选择人员" }]
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
        staffList: [],
        liveShot: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
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
            types2: [
              { value: "厨房", text: "厨房" },
              { value: "餐厅", text: "餐厅" }
            ],
            types1: [
              { value: "荤", text: "荤" },
              { value: "素", text: "素" },
              { value: "作料", text: "作料" },
              { value: "其他", text: "其他" }
            ],
            dateType: [
              { value: "morn", text: "早餐" },
              { value: "noon", text: "中餐" },
              { value: "night", text: "晚餐" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "菜名", key: "cuisineName", sortable: true },
            { title: "主料", key: "ingredient",ellipsis:true },
            { title: "配料", key: "burdening",ellipsis:true },
            { title: "简介", key: "abstract", ellipsis:true},
            { title: "用餐时间", key: "datetime" },
            { title: "用餐类型", key: "datetype" },
            { title: "添加时间", key: "addTime" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
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
      rolelist: [],
      departmentlist: [],
      schoolList: []
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建每餐实拍";
      }
      if (this.formModel.mode === "edit") {
        return "编辑每餐实拍";
      }
      if (this.formModel.mode === "show") {
        let s="每餐实拍详情";
        if(this.formModel.fields.schoolName!=""&&this.$store.state.user.schoolguid==null){
          s+="   学校:("+this.formModel.fields.schoolName+")";
        }
        
        return s;
        // return "每餐实拍详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.liveShotUuid);
    }
  },
  methods: {
    loadLiveShotList() {
      console.log(this.stores.liveShot.query.kw2);
      getLiveShotList(this.stores.liveShot.query).then(res => {
        console.log(res.data.data);
        this.stores.liveShot.data = res.data.data;
        this.stores.liveShot.query.totalCount = res.data.totalCount;
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
    handleSwitchFormModeToShow() {
      this.formModel.mode = "show";
      this.handleOpenFormWindow();
    },
    async handleEdit(row) {
      // this.defaultfilelist = [];
      this.formModel.dFileName = "";
      this.$refs.upload.fileList = [
        { name: "", status: "finished", showProgress: false }
      ];
      // await this.loadCuiSineList();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormLiveShot();
      await this.doLoadLiveShot(row.liveShotUuid);
    },
    handleShow(row) {
      // this.loadCuiSineList();
      this.handleSwitchFormModeToShow();
      this.handleResetFormLiveShot();
      this.doLoadLiveShot(row.liveShotUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadLiveShotList();
    },
    handleShowCreateWindow() {
      this.uploadList=[];
      console.log(this.formModel.fields.datetime);
      this.$refs.upload.clearFiles();
      // this.loadCuiSineList(null, "morn");
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormLiveShot();
      // this.formModel.fields.datetime = DateToString(new Date());
      this.formModel.fields.datetype = "morn";
    },
    handleSubmitLiveShot(num) {
      console.log(this.formModel.fields.accessory);
      if (
        this.formModel.fields.accessory == null ||
        this.formModel.fields.accessory == ""
      ) {
        this.$Message.warning("请上传图片");
        return;
      }
      let valid = this.validateLiveShotForm();
      //console.log(valid);
      console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateLiveShot(num);
        }
        if (this.formModel.mode === "edit") {
          this.doEditLiveShot(num);
        }
      }
    },
    handleResetFormLiveShot() {
      this.$refs.formLiveShot.resetFields();
      // this.$refs["formLiveShot2"].resetFields();
      this.formModel.fields.accessory = "";
      this.formModel.fields.liveShotUuid = "";
      this.formModel.fields.schoolUuid = "";
    },
    doCreateLiveShot(num) {
      console.log(this.$store.state.user);
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;

      createLiveShot(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadLiveShotList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditLiveShot(num) {
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      editLiveShot(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadLiveShotList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateLiveShotForm() {
      let _valid = false;
      this.$refs["formLiveShot"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          return _valid;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    async doLoadLiveShot(uuid) {
      await loadLiveShot({ guid: uuid }).then(res => {
        console.log(res.data.data);
        if (this.formModel.mode != "create") {
          console.log(res.data.data.entity.datetime);
          this.loadCuiSineList2(
            res.data.data.entity.datetime,
            res.data.data.entity.datetype
          );
        }
        this.formModel.fields = res.data.data.entity;
        console.log(1111);
        console.log(this.formModel.fields.accessory);
        let list = this.formModel.fields.accessory.split("|");
        this.uploadList = [];
        for (let i = 0; i < list.length; i++) {
          this.uploadList.push({
            url: config.baseUrl.dev + "UploadFiles/LiveShotPicture/" + list[i],
            status: "finished",
            name: "UploadFiles/LiveShotPicture/" + list[i],
            fileName: list[i]
          });
        }
        // this.$refs.upload.fileList[0].name = this.formModel.fields.accessory.split(
        //   "\\"
        // )[1];
        // this.formModel.dFileName = res.data.data.path;
        // this.url =
        //   config.baseUrl.dev +
        //   this.formModel.fields.accessory.replace("\\", "/");
        //this.formModel.selectPeople = res.data.data.systemUserUuid.split(",");
        // console.log(this.defaultfilelist);
      });
    },
    handleDelete(row) {
      this.doDelete(row.liveShotUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteLiveShot(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadLiveShotList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
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
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadLiveShotList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchLiveShot() {
      console.log(this.stores.liveShot.query.kw2);
      this.loadLiveShotList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.liveShot.query.sort.direction = column.order;
      this.stores.liveShot.query.sort.field = column.key;
      this.loadLiveShotList();
    },
    handlePageChanged(page) {
      this.stores.liveShot.query.currentPage = page;
      this.loadLiveShotList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.liveShot.query.pageSize = pageSize;
      this.loadLiveShotList();
    },
    async loadCuiSineList(time, type) {
      console.log(22222);
      let date = DateToString(time);
      await cuisineSelectList({ date: date, type: type }).then(res => {
        console.log(res);
        this.formModel.cuisines = res.data.data;
      });
    },
    async loadCuiSineList2(time, type) {
      console.log(333333);
      await cuisineSelectList({ date: time, type: type }).then(res => {
        console.log(res);
        this.formModel.cuisines = res.data.data;
      });
    },
    toSearchCuisineList() {
      let date = DateToString(this.formModel.fields.datetime);
      cuisineSelectList({
        date: date,
        type: this.formModel.fields.datetype
      }).then(res => {
        console.log(res);
        this.formModel.cuisines = res.data.data;
      });
      this.formModel.fields.cuisineUuid = "";
      this.formModel.fields.abstract = "";
      this.formModel.fields.burdening = "";
      this.formModel.fields.ingredient = "";
    },
    renderUserType(userType) {
      // console.log(userType);
      var userTypeText = "未知";
      if (userType != null && userType != "") {
        userTypeText = userType;
      }
      // switch (userType) {
      //   case 0:
      //     userTypeText = "超级管理员";
      //     break;
      //   case 1:
      //     userTypeText = "管理员";
      //     break;
      //   case 2:
      //     userTypeText = "普通用户";
      //     break;
      // }
      return userTypeText;
    },
    renderStatus(status) {
      // console.log(status);
      let _status = {
        color: "success",
        text: "已提交"
      };
      switch (status) {
        case "0":
          _status.text = "保存中";
          _status.color = "primary";
          break;
      }
      return _status;
    },
    changePeople(e) {
      console.log(e);
      this.formModel.fields.systemUserUuid = e.join();
    },
    changeCuisineName(e) {
      console.log(e);
      if (e == null || e == undefined) {
        return;
      }
      let data = this.formModel.cuisines.find(x => x.cuisineUuid == e);
      this.formModel.fields.ingredient = data.ingredient;
      this.formModel.fields.burdening = data.burdening;
      this.formModel.fields.abstract = data.abstract;
    },
    // loadStaffList() {
    //   getStaffList().then(res => {
    //     console.log(res);
    //     this.stores.staffList = res.data.data;
    //   });
    // },
    dateChage(e) {
      console.log(e);
      if (e != null && e.length == 2) {
        this.handleSearchLiveShot();
      }
    },
    checkShow() {
      return this.formModel.mode === "show";
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
        if (this.formModel.fields.accessory.length > 0) {
          this.formModel.fields.accessory += "|";
        }
        this.formModel.fields.accessory += response.data.fname;
        // this.formModel.dFileName = response.data.paths;

        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname
        });

        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(response.message);
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
    deleteFile(e) {
      console.log(e);
      console.log(this.formModel.dFileName);
      // if (this.formModel.dFileName != null && this.formModel.dFileName != "") {
      //   deletetoFile({ filename: this.formModel.dFileName }).then(res => {
      //     console.log(res);
      //   });
      // }
    },
    download() {
      // console.log(this.url);
      window.location.href =
        config.baseUrl.dev + this.formModel.fields.accessory.replace("\\", "/");
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deletetoFile({ path: file }).then(res => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join("|");
        } else {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join("|");
        }
      });
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
    }
  },
  mounted() {
    console.log(this.$store.state.user);
    this.loadLiveShotList();
    console.log(this.$refs.upload.fileList);
    //this.uploadList = this.$refs.upload.fileList;
  },
  created() {
    if(this.$store.state.user.schoolguid==null){
      this.stores.liveShot.columns.splice(1,0,{title: "学校", key: "schoolName"})
    }
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/DiningRoom/LiveShot/UpLoad";
  }
};
</script>

<style>
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
